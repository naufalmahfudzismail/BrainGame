<?php
	$connect = mysqli_connect ("localhost", "root", "", "unydb");
	if (!$connect && !$database)
	{
		die ("Tidak dapat tersambung ke Host atau database tidak ditemukan!");
	}

	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];

	$jaw = "select * from player where username = '$username' AND password = '$password'";

	$result = mysqli_query($connect, $jaw);
	$row = mysqli_fetch_row($result);

	if ($row[3])
	{
		echo "Login Berhasil. Selamat datang '$row[3]'!";
	}
	else
	{
		echo "Cek username atau password";
	}
	?>
