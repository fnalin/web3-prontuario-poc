#!/bin/bash
set -e

# Inicia o Hardhat node em segundo plano
npx hardhat node &

# Aguarda o node subir
echo "⏳ Aguardando Hardhat iniciar..."
sleep 5

# Faz deploy no node local (sem --network)
echo "🚀 Deploy do contrato em andamento..."
npx hardhat run scripts/deploy.ts

# Copia o endereço para o volume compartilhado
cp deployed.json /app/deployed/deployed.json

# Mantém o container ativo
tail -f /dev/null