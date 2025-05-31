#!/bin/bash
set -e

# Inicia o Hardhat node em segundo plano
npx hardhat node &

# Aguarda subir
echo "⏳ Aguardando Hardhat iniciar..."
sleep 5

# Faz deploy do contrato
echo "🚀 Deploy do contrato em andamento..."
npx hardhat run scripts/deploy.ts --network localhost

# Copia o endereço para o volume compartilhado
cp deployed.json /app/deployed/deployed.json

# Mantém container ativo
tail -f /dev/null