#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

dotnet restore Asp_Book.slnx --ignore-failed-sources
dotnet build Asp_Book.slnx --no-restore
dotnet test Asp_Book.Chapter12.Tests/Asp_Book.Chapter12.Tests.csproj --no-build -v minimal
