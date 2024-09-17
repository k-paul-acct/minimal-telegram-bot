#!/usr/bin/env bash

usage() {
    echo "Usage: $0 --version <version> --api-key <api-key>"
    exit 1
}

if [ $# -ne 4 ]; then
    usage
fi

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --version)
            VERSION="$2"
            shift 2
            ;;
        --api-key)
            API_KEY="$2"
            shift 2
            ;;
        *)
            usage
            ;;
    esac
done

if [ -z "$VERSION" ] || [ -z "$API_KEY" ]; then
    usage
fi

find ../src -name "*.$VERSION.nupkg" -type f | while read -r file; do
    echo "Publishing $file..."
    if ! dotnet nuget push "$file" --api-key "$API_KEY" --source https://api.nuget.org/v3/index.json; then
        echo "Failed to publish $file."
        exit 1
    else
        echo "$file published successfully."
    fi
done

echo "All packages published."
