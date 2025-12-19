#!/bin/bash

# Usage: ./find_comicinfo_in_zips.sh /path/to/search

# Exit immediately if any command fails
set -e

# Set search directory
SEARCH_DIR="${1:-.}"

# Ensure the directory exists
if [ ! -d "$SEARCH_DIR" ]; then
    echo "Error: '$SEARCH_DIR' is not a valid directory."
    exit 1
fi

echo "Scanning directory: $SEARCH_DIR"
echo "Looking for zip files containing 'Comicinfo.xml'..."

# Find all .cbz files recursively
find "$SEARCH_DIR" -type f -name "*.cbz" | while IFS= read -r zipfile; do
    # Check if the zip contains Comicinfo.xml (case-sensitive)
    if unzip -l "$zipfile" | grep -q '\bComicinfo\.xml\b'; then
        echo "✅ Found ComicInfo.xml in: $zipfile"
    fi
done

echo "Scan complete."
