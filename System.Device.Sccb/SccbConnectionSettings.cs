// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Device.Sccb
{
    /// <summary>
    /// The connection settings of a device on an Sccb bus.
    /// </summary>
    public sealed class SccbConnectionSettings
    {
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int _busId;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly SccbBusSpeed _busSpeed;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int _deviceAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="SccbConnectionSettings"/> class.
        /// </summary>
        /// <param name="busId">The bus ID the Sccb device is connected to.</param>
        /// <param name="deviceAddress">The bus address of the Sccb device.</param>
        public SccbConnectionSettings(int busId, int deviceAddress) : this(busId, deviceAddress, SccbBusSpeed.FastMode)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SccbConnectionSettings"/> class.
        /// </summary>
        /// <param name="busId">The bus ID the Sccb device is connected to.</param>
        /// <param name="deviceAddress">The bus address of the Sccb device.</param>
        /// <param name="busSpeed">The bus speed of the Sccb device.</param>
        public SccbConnectionSettings(int busId, int deviceAddress, SccbBusSpeed busSpeed)
        {
            _busId = busId;
            _deviceAddress = deviceAddress;
            _busSpeed = busSpeed;
        }

        internal SccbConnectionSettings(SccbConnectionSettings other)
        {
            _busId = other.BusId;
            _deviceAddress = other.DeviceAddress;
            _busSpeed = other.BusSpeed;
        }

        private SccbConnectionSettings()
        { }

        /// <summary>
        /// The bus ID the Sccb device is connected to.
        /// </summary>
        public int BusId { get => _busId; }

        /// <summary>
        /// The bus speed of the Sccb device
        /// </summary>
        public SccbBusSpeed BusSpeed { get => _busSpeed; }

        /// <summary>
        /// The bus address of the Sccb device.
        /// </summary>
        public int DeviceAddress { get => _deviceAddress; }
    }
}