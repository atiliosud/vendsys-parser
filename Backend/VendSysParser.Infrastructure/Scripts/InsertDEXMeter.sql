INSERT INTO DEXMeter (MachineID, DEXDateTime, MachineSerialNumber, ValueOfPaidVends)
OUTPUT INSERTED.Id
VALUES (@MachineID, @DEXDateTime, @MachineSerialNumber, @ValueOfPaidVends)