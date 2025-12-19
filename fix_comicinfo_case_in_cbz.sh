#!/bin/bash
# fix_comicinfo_case_in_cbz.sh
# This script scans all .cbz files in the current folder (or a given folder)
# and renames "Comicinfo.xml" → "ComicInfo.xml" inside each archive.

set -e

# Use first argument as directory, default to current
TARGET_DIR="${1:-.}"

# Check that directory exists
if [ ! -d "$TARGET_DIR" ]; then
    echo "Error: '$TARGET_DIR' is not a valid directory."
    exit 1
fi

echo "Processing .cbz files in: $TARGET_DIR"
echo

# Loop through all .cbz files
shopt -s nullglob
for file in "$TARGET_DIR"/*.cbz; do
    echo "Checking: $file"

    # Check if Comicinfo.xml (lowercase 'i') exists in the archive
    if unzip -l "$file" | grep -q '\bComicinfo\.xml\b'; then
        echo "  Found Comicinfo.xml — renaming to ComicInfo.xml..."

        # Create a temporary directory
        tempdir=$(mktemp -d)
        
        # Extract only the Comicinfo.xml file
        unzip -q "$file" "Comicinfo.xml" -d "$tempdir" 2>/dev/null || true

        if [ -f "$tempdir/Comicinfo.xml" ]; then
            # Rename it to ComicInfo.xml
            mv "$tempdir/Comicinfo.xml" "$tempdir/ComicInfo.xml"

            # Remove the old file from the zip
            zip -qd "$file" "Comicinfo.xml"

            # Add the renamed file
            (cd "$tempdir" && zip -q "$file" "ComicInfo.xml")

            echo "  ✅ Renamed inside archive."
        else
            echo "  ⚠️ Could not extract Comicinfo.xml (unexpected issue)."
        fi

        # Clean up
        rm -rf "$tempdir"
    else
        echo "  No Comicinfo.xml found. Skipping."
    fi

    echo
done

echo "All done."
