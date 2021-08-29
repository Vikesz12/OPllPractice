package com.viktor.ble;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothProfile;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanFilter;
import android.bluetooth.le.ScanResult;
import android.bluetooth.le.ScanSettings;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Handler;
import android.os.ParcelUuid;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;


public class Ble {
    private final static String TAG = Ble.class.getSimpleName();
    private static final int REQUEST_ENABLE_BT = 1;
    private static final int MY_PERMISSION_RESPONSE = 2;

    private static UUID SERVICE_UUID;
    private static UUID READ_CHARACTERISTIC_UUID;
    private static UUID WRITE_CHARACTERISTIC_UUID;

    private final String mGameObjName;
    private final String mCallBackName;
    private boolean scanning;

    private final BluetoothAdapter mBluetoothAdapter;
    private BluetoothLeScanner mBlueToothLeScanner;
    private BluetoothGatt mBluetoothGatt;
    private BluetoothGattCharacteristic mReadCharacteristic;
    private BluetoothGattCharacteristic mWriteCharacteristic;
    private Handler mHandler;

    // Stops scanning after 15 seconds.
    private static final long SCAN_PERIOD = 15000;

    public Ble(String gameObjName, String callBackName) {

        mGameObjName = gameObjName;
        mCallBackName = callBackName;

        mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        if (mBluetoothAdapter == null) {
            Log.d(TAG, "No bluetooth device");
            return;
        }

        if (!mBluetoothAdapter.isEnabled()) {
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            UnityPlayer.currentActivity.startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
        }

        mBlueToothLeScanner = this.mBluetoothAdapter.getBluetoothLeScanner();
        mHandler = new Handler();
    }

    public void setSERVICE_UUID(String SERVICE_UUID) {
        Ble.SERVICE_UUID = UUID.fromString(SERVICE_UUID);
    }

    public void setREAD_CHARACTERISTIC_UUID(String READ_CHARACTERISTIC_UUID) {
        Ble.READ_CHARACTERISTIC_UUID = UUID.fromString(READ_CHARACTERISTIC_UUID);
    }

    public void setWRITE_CHARACTERISTIC_UUID(String WRITE_CHARACTERISTIC_UUID) {
        Ble.WRITE_CHARACTERISTIC_UUID = UUID.fromString(WRITE_CHARACTERISTIC_UUID);
    }

    public void scanLeDevice() {
        if (!scanning) {
            // Stops scanning after a predefined scan period.
            mHandler.postDelayed(() -> {
                scanning = false;
                mBlueToothLeScanner.stopScan(leScanCallback);
            }, SCAN_PERIOD);

            scanning = true;
            Log.i(TAG, "Started scanning for uuid: " +SERVICE_UUID);

            ScanSettings settings = new ScanSettings.Builder().setScanMode(1).build();

            ScanFilter filter = new ScanFilter.Builder()
                    .setServiceUuid(new ParcelUuid(SERVICE_UUID))
                    .build();
            List<ScanFilter> filters = new ArrayList<>();
            filters.add(filter);

            mBlueToothLeScanner.startScan(filters, settings, leScanCallback);
        } else {
            scanning = false;
            mBlueToothLeScanner.stopScan(leScanCallback);
        }
    }

    // Device scan callback.
    private final ScanCallback leScanCallback =
            new ScanCallback() {
                @Override
                public void onScanResult(int callbackType, ScanResult result) {
                    super.onScanResult(callbackType, result);
                    String address = result.getDevice().getAddress();
                    String name = result.getDevice().getName();
                    Log.i(TAG, "Found device" + address + " " + name);
                    UnityPlayer.UnitySendMessage(mGameObjName,
                            mCallBackName,
                            "device|" + address + "|" + name);
                }
            };

    private boolean connect(String address) {
        if (mBluetoothAdapter == null || address == null) {
            Log.w(TAG, "BluetoothAdapter not initialized or unspecified address.");
            return false;
        }

        // Previously connected device.  Try to reconnect.
        if (mBluetoothGatt != null) {
            Log.d(TAG, "Trying to use an existing mBluetoothGatt for connection.");
            return mBluetoothGatt.connect();
        }

        try {
            BluetoothDevice device = mBluetoothAdapter.getRemoteDevice(address);
            mBluetoothGatt = device.connectGatt(UnityPlayer.currentActivity, false, mGattCallback);
            Log.d(TAG, "Trying to create a new connection.");
            return true;
        } catch (IllegalArgumentException exception) {
            Log.w(TAG, "Device not found with provided address.");
            return false;
        }
    }

    public void disconnect() {
        if (mBluetoothGatt == null) {
            return;
        }

        mBluetoothGatt.close();
        mBluetoothGatt = null;
    }

    public boolean write(byte[] data) {
        if (mWriteCharacteristic == null) {
            Log.i(TAG, "Write characteristic is undefined.");
            return false;
        }
        mWriteCharacteristic.setValue(data);
        return mBluetoothGatt.writeCharacteristic(mReadCharacteristic);
    }


    private final BluetoothGattCallback mGattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            if (newState == BluetoothProfile.STATE_CONNECTED) {
                Log.i(TAG, "Connected to GATT server.");

                // Attempts to discover services after successful connection.
                mBluetoothGatt.discoverServices();
            } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                Log.i(TAG, "Disconnected from GATT server.");
            } else {
                Log.i(TAG, "onConnectionStateChange:" + newState);
            }
        }

        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                mReadCharacteristic = gatt.getService(SERVICE_UUID).
                        getCharacteristic(READ_CHARACTERISTIC_UUID);
                mWriteCharacteristic = gatt.getService(SERVICE_UUID).
                        getCharacteristic(WRITE_CHARACTERISTIC_UUID);

                gatt.setCharacteristicNotification(
                        mReadCharacteristic,
                        true
                );
            } else {
                Log.w(TAG, "onServicesDiscovered received: " + status);
            }
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt,
                                            BluetoothGattCharacteristic characteristic) {
            UnityPlayer.UnitySendMessage(mGameObjName,
                    mCallBackName,
                    "notification|" + new String(characteristic.getValue()));
        }
    };
}
