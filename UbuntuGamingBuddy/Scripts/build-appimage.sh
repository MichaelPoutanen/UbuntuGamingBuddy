#!/usr/bin/env bash
set -euo pipefail

# --- locate project root ---
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_ROOT"
echo "📂 Running from project root: $PROJECT_ROOT"

# --- verify that a .csproj exists ---
found_csproj=false
for f in *.csproj; do
  if [[ -f "$f" ]]; then
    found_csproj=true
    break
  fi
done

if [[ "$found_csproj" == false ]]; then
  echo "❌ No .csproj file found in $PROJECT_ROOT"
  exit 1
fi

PROJECT_ROOT="$PWD"
echo "📂 Running from project root: $PROJECT_ROOT"

APP_NAME="UbuntuGamingBuddy"
RUNTIME="linux-x64"
DIST_DIR="dist"
APPDIR="${APP_NAME}.AppDir"

echo "🚀 Building ${APP_NAME} AppImage..."

# Clean old builds
rm -rf "$APPDIR" "$DIST_DIR"
mkdir -p "$APPDIR/usr/bin" "$DIST_DIR"

# Publish (self-contained single file)
echo "📦 Publishing project..."
dotnet publish -c Release -r $RUNTIME --self-contained true \
  /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

# Find the publish directory (search under bin/Release/*/*/publish)
PUBLISH_DIR="$(find bin/Release -type d -path "*/publish" -print -quit)"
if [[ -z "$PUBLISH_DIR" ]]; then
  echo "❌ Could not find publish directory under bin/Release. Aborting."
  exit 1
fi
echo "ℹ️  Found publish directory: $PUBLISH_DIR"

# Try to locate an executable in the publish dir
PUBLISHED_EXE="$(find "$PUBLISH_DIR" -maxdepth 1 -type f -executable -printf '%f\n' | head -n 1 || true)"

# If none found, try to find a file that starts with APP_NAME
if [[ -z "$PUBLISHED_EXE" ]]; then
  PUBLISHED_EXE="$(ls -1 "$PUBLISH_DIR" | grep -i "^${APP_NAME}" | head -n1 || true)"
fi

if [[ -z "$PUBLISHED_EXE" ]]; then
  echo "❌ Could not find the published executable in $PUBLISH_DIR."
  echo "Contents of publish dir:"
  ls -la "$PUBLISH_DIR"
  exit 1
fi

echo "ℹ️  Using published executable: $PUBLISHED_EXE"

# Copy published binary into AppDir
cp "$PUBLISH_DIR/$PUBLISHED_EXE" "${APPDIR}/usr/bin/${APP_NAME}"
chmod +x "${APPDIR}/usr/bin/${APP_NAME}"

# Create AppRun
cat << 'EOF' > "${APPDIR}/AppRun"
#!/bin/bash
HERE="$(dirname "$(readlink -f "${0}")")"
exec "$HERE/usr/bin/UbuntuGamingBuddy" "$@"
EOF
chmod +x "${APPDIR}/AppRun"

# .desktop
cat << EOF > "${APPDIR}/${APP_NAME}.desktop"
[Desktop Entry]
Name=Ubuntu Gaming Buddy
Exec=${APP_NAME}
Icon=ubuntu-gaming-buddy
Type=Application
Categories=Game
Comment=A helper app to make it more comfortable to play on Ubuntu using a streaming service or Remote desktop
EOF

# Copy icon
if [[ -f "Assets/icon_ubuntuGamingBuddy_on.png" ]]; then
  cp "Assets/icon_ubuntuGamingBuddy_on.png" "${APPDIR}/ubuntu-gaming-buddy.png"
else
  echo "⚠️  Missing icon: Assets/icon_gaming_on.png (continue anyway)"
fi

# Ensure appimagetool present (download locally if needed)
if ! command -v appimagetool &> /dev/null; then
  if [[ ! -f "./appimagetool-x86_64.AppImage" ]]; then
    echo "⬇️  Downloading appimagetool..."
    wget -q https://github.com/AppImage/AppImageKit/releases/latest/download/appimagetool-x86_64.AppImage
    chmod +x appimagetool-x86_64.AppImage
  fi
  APPIMAGETOOL="./appimagetool-x86_64.AppImage"
else
  APPIMAGETOOL="appimagetool"
fi

# Build AppImage
echo "🛠️  Building AppImage..."
$APPIMAGETOOL "$APPDIR" "${DIST_DIR}/${APP_NAME}-x86_64.AppImage"

echo "✅ Build complete!"
echo "AppImage created at: ${DIST_DIR}/${APP_NAME}-x86_64.AppImage"