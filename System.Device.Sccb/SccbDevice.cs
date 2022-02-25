// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Device.Sccb
{
    /// <summary>
    /// The communications channel to a device on an Sccb bus.
    /// </summary>
    public class SccbDevice : IDisposable
    {
        // this is used as the lock object
        // a lock is required because multiple threads can access the device
        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private readonly SccbConnectionSettings _connectionSettings;

        [Diagnostics.DebuggerBrowsable(Diagnostics.DebuggerBrowsableState.Never)]
        private bool _disposed;

        // speeds up the execution of ReadByte and WriteByte operations
        private readonly byte[] _buffer;

        /// <summary>
        /// The connection settings of a device on an Sccb bus. The connection settings are immutable after the device is created
        /// so the object returned will be a clone of the settings object.
        /// </summary>
        public SccbConnectionSettings ConnectionSettings { get => _connectionSettings; }

        /// <summary>
        /// Reads a byte from the Sccb device.
        /// </summary>
        /// <returns>A byte read from the Sccb device.</returns>
        public byte ReadByte()
        {
            lock (_syncLock)
            {
                var buffer = new SpanByte(_buffer);

                NativeTransmit(null, buffer);

                return buffer[0];
            }
        }

        /// <summary>
        /// Reads data from the Sccb device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to read the data from the Sccb device.
        /// The length of the buffer determines how much data to read from the Sccb device.
        /// </param>
        public SccbTransferResult Read(SpanByte buffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(null, buffer);
            }
        }

        /// <summary>
        /// Writes a byte to the Sccb device.
        /// </summary>
        /// <param name="value">The byte to be written to the Sccb device.</param>
        public SccbTransferResult WriteByte(byte value)
        {
            lock (_syncLock)
            {
                // copy value
                _buffer[0] = value;

                return NativeTransmit(new SpanByte(_buffer), null);
            }
        }

        /// <summary>
        /// Writes data to the Sccb device.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that contains the data to be written to the Sccb device.
        /// The data should not include the Sccb device address.
        /// </param>
        public SccbTransferResult Write(SpanByte buffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(buffer, null);
            }
        }

        /// <summary>
        /// Performs an atomic operation to write data to and then read data from the Sccb bus on which the device is connected,
        /// and sends a restart condition between the write and read operations.
        /// </summary>
        /// <param name="writeBuffer">
        /// The buffer that contains the data to be written to the Sccb device.
        /// The data should not include the Sccb device address.</param>
        /// <param name="readBuffer">
        /// The buffer to read the data from the Sccb device.
        /// The length of the buffer determines how much data to read from the Sccb device.
        /// </param>
        public SccbTransferResult WriteRead(SpanByte writeBuffer, SpanByte readBuffer)
        {
            lock (_syncLock)
            {
                return NativeTransmit(writeBuffer, readBuffer);
            }
        }

        /// <summary>
        /// Creates a communications channel to a device on an Sccb bus running on the current platform
        /// </summary>
        /// <param name="settings">The connection settings of a device on an Sccb bus.</param>
        /// <returns>A communications channel to a device on an Sccb bus</returns>
        public static SccbDevice Create(SccbConnectionSettings settings)
        {
            return new SccbDevice(settings);
        }

        /// <summary>
        /// Create an Sccb Device
        /// </summary>
        /// <param name="settings">Connection settings</param>
        public SccbDevice(SccbConnectionSettings settings)
        {
            _connectionSettings = settings;

            // create the buffer
            _buffer = new byte[1];

            // create the lock object
            _syncLock = new object();

            // call native init to allow HAL/PAL inits related with Sccb hardware
            NativeInit();
        }

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                NativeDispose();

                _disposed = true;
            }
        }

#pragma warning disable 1591

        ~SccbDevice()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                if (!_disposed)
                {
                    Dispose(true);

                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion IDisposable Support

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeInit();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDispose();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern SccbTransferResult NativeTransmit(SpanByte writeBuffer, SpanByte readBuffer);

        #endregion external calls to native implementations
    }
}