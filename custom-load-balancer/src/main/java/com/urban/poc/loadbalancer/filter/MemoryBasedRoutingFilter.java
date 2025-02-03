package com.urban.poc.loadbalancer.filter;

import com.urban.poc.loadbalancer.dto.MemoryStats;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.cloud.client.ServiceInstance;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.cloud.gateway.filter.GatewayFilterChain;
import org.springframework.cloud.gateway.filter.GlobalFilter;
import org.springframework.cloud.gateway.support.ServerWebExchangeUtils;
import org.springframework.core.Ordered;
import org.springframework.core.io.buffer.DataBuffer;
import org.springframework.http.ResponseCookie;
import org.springframework.http.ResponseEntity;
import org.springframework.http.server.reactive.ServerHttpRequest;
import org.springframework.stereotype.Component;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.reactive.function.client.WebClient;
import org.springframework.web.server.ServerWebExchange;
import reactor.core.publisher.Mono;

import java.net.URI;
import java.util.Comparator;

@Component
public class MemoryBasedRoutingFilter implements GlobalFilter, Ordered {

    private final DiscoveryClient discoveryClient;
    private final RestTemplate restTemplate;

    private final WebClient webClient = WebClient.builder().build();

    @Value("${session.cookie.name:PREFERRED_INSTANCE_ID}")
    private String SESSION_COOKIE_NAME;

    @Value("${mem.uri.path:/api/memoryStats}")
    private String MEMORY_STATS_PATH;

    @Value("${instance.service.name:unique-uuid-app}")
    private String INSTANCE_SERVICE_NAME;

    private final Logger logger = LoggerFactory.getLogger(MemoryBasedRoutingFilter.class);
    public MemoryBasedRoutingFilter(DiscoveryClient discoveryClient) {
        this.discoveryClient = discoveryClient;
        this.restTemplate = new RestTemplate();
    }

    @Override
    public Mono<Void> filter(ServerWebExchange exchange, GatewayFilterChain chain) {
        // Check for an existing sticky session
        String instanceId = getInstanceIdFromCookie(exchange);
        ServerHttpRequest req;
        ServiceInstance targetInstance;
        if (instanceId != null) {
            // Try to find the instance by its ID
            targetInstance = findInstanceById(instanceId);
            if (targetInstance != null) {
                logger.debug("Instance found {}", targetInstance.getInstanceId());
            } else {
                logger.debug("Instance NOT found ");
                targetInstance = selectTargetInstance();
            }
        } else {
            // Select a new instance based on available memory
            targetInstance = selectTargetInstance();
        }

        if (targetInstance != null) {
            // Add or update the sticky session cookie
            addStickySessionCookie(exchange, targetInstance);

            String targetUri = targetInstance.getUri().toString() + exchange.getRequest().getURI().getPath().replace("/unique-uuid-api", "");
            URI newUri = URI.create(targetUri);

            logger.debug("Routing request to instance ID {} and URI {}", targetInstance.getInstanceId(), newUri);

            exchange.getRequest().mutate().uri(newUri).build();

            // Set the new URI in the exchange attributes
            exchange.getAttributes().put(ServerWebExchangeUtils.GATEWAY_REQUEST_URL_ATTR, newUri);

            req = exchange.getRequest().mutate().uri(newUri).build();


            return webClient.method(req.getMethod())
                    .uri(newUri)
                    .headers(headers -> headers.addAll(req.getHeaders()))
                    .exchangeToMono(clientResponse -> {
                        logger.debug("Received response with status {}", clientResponse.statusCode());
                        return exchange.getResponse().writeWith(clientResponse.bodyToMono(DataBuffer.class));
                    })
                    .doOnError(error -> logger.error("Error forwarding request", error));

        } else {
            req = null;
        }

        return chain.filter(exchange);

    }

    private String getInstanceIdFromCookie(ServerWebExchange exchange) {
        return exchange.getRequest().getCookies().getFirst(SESSION_COOKIE_NAME) != null
                ? exchange.getRequest().getCookies().getFirst(SESSION_COOKIE_NAME).getValue()
                : null;
    }

    private ServiceInstance findInstanceById(String instanceId) {
        return discoveryClient.getInstances(INSTANCE_SERVICE_NAME).stream()
                .filter(instance -> instance.getInstanceId().equals(instanceId))
                .findFirst()
                .orElse(null);
    }

    private ServiceInstance selectTargetInstance() {
        return discoveryClient.getInstances("unique-uuid-api").stream()
                .max(Comparator.comparing(this::getAvailableMemory))
                .orElse(null);
    }

    private long getAvailableMemory(ServiceInstance instance) {
        try {
            String url = instance.getUri().toString() + MEMORY_STATS_PATH;
            ResponseEntity<MemoryStats> response = restTemplate.getForEntity(url, MemoryStats.class);
            if (response.getStatusCode().is2xxSuccessful() && response.getBody() != null) {

                return response.getBody().getAvailableMB();
            }
        } catch (Exception e) {
            logger.error("Failed to retrieve memory stats from {}: {}", instance.getUri(), e.getMessage());
        }

        return 0; // Lowest priority for instances with unavailable stats
    }

    private void addStickySessionCookie(ServerWebExchange exchange, ServiceInstance instance) {
        ResponseCookie cookie = ResponseCookie.from(SESSION_COOKIE_NAME, instance.getInstanceId())
                .path("/")
                .httpOnly(true)
                .build();
        exchange.getResponse().addCookie(cookie);
    }

    @Override
    public int getOrder() {
        return -1; // High priority
    }
}
