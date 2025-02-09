package com.viktor.ble

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Log
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat

class BleActivity : UnityPlayerActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState)
        if (!isPermissionsGranted(this)) {

            val permissions = mutableSetOf(
                Manifest.permission.BLUETOOTH,
                Manifest.permission.BLUETOOTH_ADMIN,
                Manifest.permission.ACCESS_COARSE_LOCATION,
                Manifest.permission.ACCESS_FINE_LOCATION,
                Manifest.permission.BLUETOOTH_CONNECT,
                Manifest.permission.BLUETOOTH_SCAN,
            )

            ActivityCompat.requestPermissions(
                this, permissions.toTypedArray(), 600
            )
        }

        // print debug message to logcat
        Log.d("BleActivity", "onCreate called!")
    }

    private fun isPermissionsGranted(context: Context): Boolean {
        return ContextCompat.checkSelfPermission(
            context,
            Manifest.permission.ACCESS_FINE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED &&
                ContextCompat.checkSelfPermission(
                    context,
                    Manifest.permission.BLUETOOTH_CONNECT
                ) == PackageManager.PERMISSION_GRANTED &&
                ContextCompat.checkSelfPermission(
                    context,
                    Manifest.permission.BLUETOOTH_SCAN
                ) == PackageManager.PERMISSION_GRANTED
    }

    companion object {
        private const val PERMISSION_REQUEST_COARSE_LOCATION = 1
        private const val PERMISSION_REQUEST_FINE_LOCATION = 99
    }
}
