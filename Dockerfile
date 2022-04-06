FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS base
RUN curl -sL https://raw.githubusercontent.com/nvm-sh/nvm/v0.34.0/install.sh -o install_nvm.sh
RUN bash install_nvm.sh
RUN . ~/.bashrc && nvm --version && nvm install 14.18.1 && nvm use 14.18.1
