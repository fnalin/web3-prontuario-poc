// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract MedicalRecord {
    struct Record {
        bytes32 dataHash; // hash dos dados armazenados fora da blockchain
        address owner;
        mapping(address => bool) accessGranted;
    }

    mapping(address => Record) private records;

    event RecordCreated(address indexed patient, bytes32 dataHash);
    event AccessGranted(address indexed patient, address indexed doctor);
    event AccessRevoked(address indexed patient, address indexed doctor);

    function createRecord(bytes32 dataHash) external {
        Record storage r = records[msg.sender];
        r.dataHash = dataHash;
        r.owner = msg.sender;

        emit RecordCreated(msg.sender, dataHash);
    }

    function grantAccess(address doctor) external {
        require(records[msg.sender].owner == msg.sender, "Not owner");
        records[msg.sender].accessGranted[doctor] = true;
        emit AccessGranted(msg.sender, doctor);
    }

    function revokeAccess(address doctor) external {
        require(records[msg.sender].owner == msg.sender, "Not owner");
        records[msg.sender].accessGranted[doctor] = false;
        emit AccessRevoked(msg.sender, doctor);
    }

    function getRecordHash(address patient) external view returns (bytes32) {
        Record storage r = records[patient];
        require(
            r.owner == msg.sender || r.accessGranted[msg.sender],
            "Access denied"
        );
        return r.dataHash;
    }
}