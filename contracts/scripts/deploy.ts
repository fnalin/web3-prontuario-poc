import { ethers } from "hardhat";
import fs from "fs";

async function main() {
    const MedicalRecord = await ethers.getContractFactory("MedicalRecord");
    const contract = await MedicalRecord.deploy();

    // Aguarda o deployment ser confirmado
    await contract.waitForDeployment();

    // Obtém o endereço
    const address = await contract.getAddress();
    console.log(`✅ MedicalRecord deployed at: ${address}`);

    fs.writeFileSync(
        "deployed.json",
        JSON.stringify({ address }, null, 2)
    );
}



main().catch((error) => {
    console.error("❌ Deploy failed:", error);
    process.exitCode = 1;
});