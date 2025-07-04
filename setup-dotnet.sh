#!/bin/bash

# Set .NET version
DOTNET_VERSION=9.0.204

# Install dependencies
apt-get update
apt-get install -y wget zlib1g ca-certificates libc6 libgcc-s1 libicu74 libssl3 libstdc++6 libunwind8

# Download and install .NET SDK
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version $DOTNET_VERSION

# Clean up
rm -f dotnet-install.sh

# Set environment variables
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> ~/.bashrc
source ~/.bashrc
