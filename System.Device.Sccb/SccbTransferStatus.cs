// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Device.Sccb
{
    /// <summary>
    /// Describes whether the data transfers that the <see cref="SccbDevice.Read"/>, <see cref="SccbDevice.Write"/>, or <see cref="SccbDevice.WriteRead"/> methods performed succeeded, or provides the reason that the transfers did not succeed.
    /// </summary>
	public enum SccbTransferStatus
    {
        /// <summary>
        /// The transfer failed for an unknown reason.
        /// </summary>
        UnknownError,

        /// <summary>
        /// The data was entirely transferred. For WriteRead, the data for both the write and the read operations was entirely transferred.
        /// For this status code, the value of the <see cref="SccbTransferResult.BytesTransferred"/> member that the method returns is the same as the size of the buffer
        /// you specified when you called the method, or is equal to the sum of the sizes of two buffers that you specified for WriteRead.
        /// </summary>
        FullTransfer,

        /// <summary>
        /// The transfer failed due to the clock being stretched for too long. Ensure the clock line is not being held low.
        /// </summary>
        ClockStretchTimeout,

        /// <summary>
        /// The Sccb device negatively acknowledged the data transfer before all of the data was transferred.
        /// For this status code, the value of the <see cref="SccbTransferResult.BytesTransferred"/> member that the method returns is the number of bytes actually transferred.
        /// For <see cref="SccbDevice.WriteRead"/>, the value is the sum of the number of bytes that the operation wrote and the number of bytes that the operation read.
        /// </summary>
        PartialTransfer,

        /// <summary>
        /// The bus address was not acknowledged. For this status code, the value of the <see cref="SccbTransferResult.BytesTransferred"/> member that the method returns of the method is 0.
        /// </summary>
        SlaveAddressNotAcknowledged,
    }
}