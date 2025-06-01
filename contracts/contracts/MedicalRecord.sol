// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "hardhat/console.sol";

contract MedicalRecord {
    struct Record {
        address patient;
        address doctor;
        bytes32 dataHash;
        uint256 timestamp;
    }

    Record[] public records;

    // Mapeamento de existência baseado em (paciente, médico, timestamp)
    mapping(bytes32 => bool) private recordExists;

    event RecordCreated(address indexed patient, address indexed doctor, bytes32 dataHash, uint256 timestamp);

    function createRecord(address patient, bytes32 dataHash, uint256 timestamp) external {
    bytes32 key = keccak256(abi.encodePacked(patient, msg.sender, timestamp));

    console.log("patient:", patient);
    console.log("doctor (msg.sender):", msg.sender);
    console.log("timestamp:", timestamp);
    console.logBytes32(dataHash);
    console.logBytes32(key);

    require(!recordExists[key], "Record already exists");

    records.push(Record({
        patient: patient,
        doctor: msg.sender,
        dataHash: dataHash,
        timestamp: timestamp
    }));

    recordExists[key] = true;

    emit RecordCreated(patient, msg.sender, dataHash, timestamp);
}

    // 🔍 Total de registros
    function getRecordCount() external view returns (uint256) {
        return records.length;
    }

    // 🔍 Todos os registros de um paciente
    function getRecordsByPatient(address patient) external view returns (Record[] memory) {
        uint256 count = 0;
        for (uint256 i = 0; i < records.length; i++) {
            if (records[i].patient == patient) {
                count++;
            }
        }

        Record[] memory result = new Record[](count);
        uint256 j = 0;
        for (uint256 i = 0; i < records.length; i++) {
            if (records[i].patient == patient) {
                result[j] = records[i];
                j++;
            }
        }

        return result;
    }

    // 🔍 Todos os registros de um médico
    function getRecordsByDoctor(address doctor) external view returns (Record[] memory) {
        uint256 count = 0;
        for (uint256 i = 0; i < records.length; i++) {
            if (records[i].doctor == doctor) {
                count++;
            }
        }

        Record[] memory result = new Record[](count);
        uint256 j = 0;
        for (uint256 i = 0; i < records.length; i++) {
            if (records[i].doctor == doctor) {
                result[j] = records[i];
                j++;
            }
        }

        return result;
    }

    // 🔍 Gerar o hash de um registro (sem armazenar)
    function getRecordHash(address patient, address doctor, uint256 timestamp) public pure returns (bytes32) {
        return keccak256(abi.encodePacked(patient, doctor, timestamp));
    }

    // 🔍 Verifica se existe um registro com base no hash
    function recordExistsHash(bytes32 recordHash) public view returns (bool) {
        return recordExists[recordHash];
    }

    // 🔍 Verifica se existe um registro baseado nos parâmetros
    function hasRecord(address patient, address doctor, uint256 timestamp) public view returns (bool) {
        bytes32 key = keccak256(abi.encodePacked(patient, doctor, timestamp));
        return recordExists[key];
    }
}