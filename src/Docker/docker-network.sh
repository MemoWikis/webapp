docker network rm mem-stage || true
docker network create \
  --label com.docker.compose.network=environment-specific-network \
  mem-stage

docker network rm mem-prod || true
docker network create \
  --label com.docker.compose.network=environment-specific-network \
  mem-prod