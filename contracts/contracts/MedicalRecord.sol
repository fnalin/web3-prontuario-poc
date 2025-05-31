// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract MedicalRecord {
    struct Record {
        bytes32 dataHash;
        address owner;
        mapping(address => bool) accessGranted;
        address[] grantedDoctors;
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
        Record storage r = records[msg.sender];
        require(r.owner == msg.sender, "Not owner");

        if (!r.accessGranted[doctor]) {
            r.accessGranted[doctor] = true;
            r.grantedDoctors.push(doctor);
            emit AccessGranted(msg.sender, doctor);
        }
    }

    function revokeAccess(address doctor) external {
        Record storage r = records[msg.sender];
        require(r.owner == msg.sender, "Not owner");

        if (r.accessGranted[doctor]) {
            r.accessGranted[doctor] = false;
            emit AccessRevoked(msg.sender, doctor);
        }
    }

    function getRecordHash(address patient) external view returns (bytes32) {
        Record storage r = records[patient];
        require(
            r.owner == msg.sender || r.accessGranted[msg.sender],
            "Access denied"
        );
        return r.dataHash;
    }

    function getGrantedDoctors(address patient) external view returns (address[] memory) {
        Record storage r = records[patient];
        require(
            r.owner == msg.sender || r.accessGranted[msg.sender],
            "Access denied"
        );
        return r.grantedDoctors;
    }
}