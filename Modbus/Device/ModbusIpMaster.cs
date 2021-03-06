using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Net.Sockets;
using Modbus.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus IP master device.
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
	public class ModbusIpMaster : ModbusMaster
	{

        private static byte ModbusId;

		private ModbusIpMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		/// <summary>
		/// Modbus IP master factory method.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
		public static ModbusIpMaster CreateIp(TcpClient tcpClient)
		{
			if (tcpClient == null)
				throw new ArgumentNullException("tcpClient");
            ModbusId = Modbus.DefaultIpSlaveUnitId;
			return CreateIp(new TcpClientAdapter(tcpClient));
		}

        //ram fixed 7/17/2013
        /// <summary>
        /// Modbus IP master factory method with modbus ID
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
        public static ModbusIpMaster CreateIp(TcpClient tcpClient, byte modbusId)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            ModbusIpMaster.ModbusId = modbusId;

            return CreateIp(new TcpClientAdapter(tcpClient));
        }

		/// <summary>
		/// Modbus IP master factory method.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
		public static ModbusIpMaster CreateIp(UdpClient udpClient)
		{
		    if (udpClient == null)
		        throw new ArgumentNullException("udpClient");
		    if (!udpClient.Client.Connected)
		        throw new InvalidOperationException(Resources.UdpClientNotConnected);

		    return CreateIp(new UdpClientAdapter(udpClient));
		}

		/// <summary>
		/// Modbus IP master factory method.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
		public static ModbusIpMaster CreateIp(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			return CreateIp(new SerialPortAdapter(serialPort));
		}

		/// <summary>
		/// Modbus IP master factory method.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
		public static ModbusIpMaster CreateIp(IStreamResource streamResource)
		{
			if (streamResource == null)
				throw new ArgumentNullException("streamResource");

			return new ModbusIpMaster(new ModbusIpTransport(streamResource));
		}

		/// <summary>
		/// Read from 1 to 2000 contiguous coils status.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of coils to read.</param>
		/// <returns>Coils status</returns>
		public bool[] ReadCoils(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadCoils(ModbusId, startAddress, numberOfPoints);
		}

		/// <summary>
		/// Read from 1 to 2000 contiguous discrete input status.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of discrete inputs to read.</param>
		/// <returns>Discrete inputs status</returns>
		public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadInputs(ModbusId, startAddress, numberOfPoints);
		}

		/// <summary>
		/// Read contiguous block of holding registers.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of holding registers to read.</param>
		/// <returns>Holding registers status</returns>
		public ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(ModbusId, startAddress, numberOfPoints);
		}

		/// <summary>
		/// Read contiguous block of input registers.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of holding registers to read.</param>
		/// <returns>Input registers status</returns>
		public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadInputRegisters(ModbusId, startAddress, numberOfPoints);
		}

		/// <summary>
		/// Write a single coil value.
		/// </summary>
		/// <param name="coilAddress">Address to write value to.</param>
		/// <param name="value">Value to write.</param>
		public void WriteSingleCoil(ushort coilAddress, bool value)
		{
			base.WriteSingleCoil(ModbusId, coilAddress, value);
		}

		/// <summary>
		/// Write a single holding register.
		/// </summary>
		/// <param name="registerAddress">Address to write.</param>
		/// <param name="value">Value to write.</param>
		public void WriteSingleRegister(ushort registerAddress, ushort value)
		{
			base.WriteSingleRegister(ModbusId, registerAddress, value);
		}

		/// <summary>
		/// Write a block of 1 to 123 contiguous registers.
		/// </summary>
		/// <param name="startAddress">Address to begin writing values.</param>
		/// <param name="data">Values to write.</param>
		public void WriteMultipleRegisters(ushort startAddress, ushort[] data)
		{
			base.WriteMultipleRegisters(ModbusId, startAddress, data);
		}

		/// <summary>
		/// Force each coil in a sequence of coils to a provided value.
		/// </summary>
		/// <param name="startAddress">Address to begin writing values.</param>
		/// <param name="data">Values to write.</param>
		public void WriteMultipleCoils(ushort startAddress, bool[] data)
		{
			base.WriteMultipleCoils(ModbusId, startAddress, data);
		}

		/// <summary>
		/// Performs a combination of one read operation and one write operation in a single MODBUS transaction. 
		/// The write operation is performed before the read.
		/// Message uses default TCP slave id of 0.
		/// </summary>
		/// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
		/// <param name="numberOfPointsToRead">Number of registers to read.</param>
		/// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
		/// <param name="writeData">Register values to write.</param>
		public ushort[] ReadWriteMultipleRegisters(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
		{
			return base.ReadWriteMultipleRegisters(ModbusId, startReadAddress, numberOfPointsToRead, startWriteAddress, writeData);
		}
	}
}
