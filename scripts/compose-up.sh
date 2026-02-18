#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

# Docker Desktop with low RAM can fail when building many services in parallel.
export COMPOSE_PARALLEL_LIMIT="${COMPOSE_PARALLEL_LIMIT:-2}"

docker compose --parallel "$COMPOSE_PARALLEL_LIMIT" up --build --force-recreate "$@"
