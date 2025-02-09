package com.viktor.ble

import android.annotation.SuppressLint
import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothGatt
import android.bluetooth.BluetoothGattCallback
import android.bluetooth.BluetoothGattCharacteristic
import android.bluetooth.BluetoothGattCharacteristic.WRITE_TYPE_DEFAULT
import android.bluetooth.BluetoothGattDescriptor
import android.bluetooth.BluetoothProfile
import android.bluetooth.BluetoothStatusCodes
import android.bluetooth.le.BluetoothLeScanner
import android.bluetooth.le.ScanCallback
import android.bluetooth.le.ScanFilter
import android.bluetooth.le.ScanResult
import android.bluetooth.le.ScanSettings
import android.content.Intent
import android.os.Handler
import android.os.ParcelUuid
import android.util.Base64
import android.util.Log
import com.unity3d.player.UnityPlayer
import java.util.UUID

@SuppressLint("MissingPermission")
class Ble(private val mGameObjName: String, private val mCallBackName: String) {
    private var scanning = false

    private val mBluetoothAdapter: BluetoothAdapter? = BluetoothAdapter.getDefaultAdapter()
    private var mBlueToothLeScanner: BluetoothLeScanner? = null
    private var mBluetoothGatt: BluetoothGatt? = null
    private var mReadCharacteristic: BluetoothGattCharacteristic? = null
    private var mWriteCharacteristic: BluetoothGattCharacteristic? = null
    private val mHandler: Handler

    init {
        if (mBluetoothAdapter == null) {
            Log.d(TAG, "No bluetooth device")
        } else {
            if (!mBluetoothAdapter.isEnabled) {
                val enableBtIntent = Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE)
                UnityPlayer.currentActivity.startActivityForResult(
                    enableBtIntent,
                    REQUEST_ENABLE_BT
                )
            }

            mBlueToothLeScanner = mBluetoothAdapter.getBluetoothLeScanner()
        }
        mHandler = Handler()
    }

    fun setSERVICE_UUID(SERVICE_UUID: String?) {
        Companion.SERVICE_UUID = UUID.fromString(SERVICE_UUID)
    }

    fun setREAD_CHARACTERISTIC_UUID(READ_CHARACTERISTIC_UUID: String?) {
        Companion.READ_CHARACTERISTIC_UUID = UUID.fromString(READ_CHARACTERISTIC_UUID)
    }

    fun setWRITE_CHARACTERISTIC_UUID(WRITE_CHARACTERISTIC_UUID: String?) {
        Companion.WRITE_CHARACTERISTIC_UUID = UUID.fromString(WRITE_CHARACTERISTIC_UUID)
    }

    fun setNOTIFY_DESCRIPTOR_UUID(NOTIFY_DESCRIPTOR_UUID: String?) {
        Companion.NOTIFY_DESCRIPTOR_UUID = UUID.fromString(NOTIFY_DESCRIPTOR_UUID)
    }

    fun scanLeDevice() {
        if (!scanning) {
            // Stops scanning after a predefined scan period.
            mHandler.postDelayed({
                scanning = false
                mBlueToothLeScanner?.stopScan(leScanCallback)
            }, SCAN_PERIOD)

            scanning = true
            Log.i(TAG, "Started scanning for uuid: " + SERVICE_UUID)

            val settings = ScanSettings.Builder().setScanMode(1).build()

            val filter = ScanFilter.Builder()
                .setServiceUuid(ParcelUuid(SERVICE_UUID))
                .build()
            val filters: MutableList<ScanFilter> = ArrayList()
            filters.add(filter)

            mBlueToothLeScanner?.startScan(filters, settings, leScanCallback)
        } else {
            scanning = false
            mBlueToothLeScanner?.stopScan(leScanCallback)
        }
    }

    // Device scan callback.
    private val leScanCallback: ScanCallback = object : ScanCallback() {
        override fun onScanResult(callbackType: Int, result: ScanResult) {
            super.onScanResult(callbackType, result)
            val address = result.device.address
            val name = result.device.name
            Log.i(TAG, "Found device$address $name")
            UnityPlayer.UnitySendMessage(
                mGameObjName,
                mCallBackName,
                "device|$address|$name"
            )
        }
    }

    private fun connect(address: String?): Boolean {
        if (mBluetoothAdapter == null || address == null) {
            Log.w(TAG, "BluetoothAdapter not initialized or unspecified address.")
            return false
        }

        // Previously connected device.  Try to reconnect.
        if (mBluetoothGatt != null) {
            Log.d(TAG, "Trying to use an existing mBluetoothGatt for connection.")
            return mBluetoothGatt!!.connect()
        }

        try {
            val device = mBluetoothAdapter.getRemoteDevice(address)
            mBluetoothGatt = device.connectGatt(UnityPlayer.currentActivity, false, mGattCallback)
            Log.d(TAG, "Trying to create a new connection.")
            return true
        } catch (exception: IllegalArgumentException) {
            Log.w(TAG, "Device not found with provided address.")
            return false
        }
    }

    fun disconnect() {
        if (mBluetoothGatt == null) {
            return
        }

        mBluetoothGatt!!.close()
        mBluetoothGatt = null
    }

    fun write(data: ByteArray?): Boolean {
        if (mWriteCharacteristic == null || data == null) {
            Log.i(TAG, "Write characteristic is undefined.")
            return false
        }
        return mBluetoothGatt!!.writeCharacteristic(
            mWriteCharacteristic!!,
            data,
            WRITE_TYPE_DEFAULT
        ) == BluetoothStatusCodes.SUCCESS
    }


    private val mGattCallback: BluetoothGattCallback = object : BluetoothGattCallback() {
        override fun onConnectionStateChange(gatt: BluetoothGatt, status: Int, newState: Int) {
            if (newState == BluetoothProfile.STATE_CONNECTED) {
                Log.i(TAG, "Connected to GATT server.")

                // Attempts to discover services after successful connection.
                mBluetoothGatt!!.discoverServices()
            } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                Log.i(TAG, "Disconnected from GATT server.")
            } else {
                Log.i(TAG, "onConnectionStateChange:$newState")
            }
        }

        override fun onServicesDiscovered(gatt: BluetoothGatt, status: Int) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                mReadCharacteristic =
                    gatt.getService(SERVICE_UUID).getCharacteristic(READ_CHARACTERISTIC_UUID)
                mWriteCharacteristic =
                    gatt.getService(SERVICE_UUID).getCharacteristic(WRITE_CHARACTERISTIC_UUID)

                Log.i(TAG, "Subscribed to: $mReadCharacteristic")
                gatt.setCharacteristicNotification(
                    mReadCharacteristic,
                    true
                )
                if (READ_CHARACTERISTIC_UUID == mReadCharacteristic?.uuid) {
                    val descriptor = mReadCharacteristic?.getDescriptor(NOTIFY_DESCRIPTOR_UUID)
                    if (descriptor != null) {
                        mBluetoothGatt!!.writeDescriptor(
                            descriptor,
                            BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE
                        )
                    }
                }
            } else {
                Log.w(TAG, "onServicesDiscovered received: $status")
            }
        }

        override fun onCharacteristicChanged(
            gatt: BluetoothGatt,
            characteristic: BluetoothGattCharacteristic,
            value: ByteArray
        ) {
            Log.i(TAG, "Noti recieved: " + value.contentToString())
            val base64 = Base64.encodeToString(value, Base64.DEFAULT)
            UnityPlayer.UnitySendMessage(
                mGameObjName,
                mCallBackName,
                "notification|$base64"
            )
        }
    }


    companion object {
        private val TAG: String = Ble::class.java.simpleName
        private const val REQUEST_ENABLE_BT = 1
        private const val MY_PERMISSION_RESPONSE = 2

        private var SERVICE_UUID: UUID? = null
        private var READ_CHARACTERISTIC_UUID: UUID? = null
        private var WRITE_CHARACTERISTIC_UUID: UUID? = null
        private var NOTIFY_DESCRIPTOR_UUID: UUID? = null

        // Stops scanning after 15 seconds.
        private const val SCAN_PERIOD: Long = 15000
    }
}
