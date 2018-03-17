<?php
	$connect = mysqli_connect ("localhost", "root", "", "unydb");
	if (!$connect && !$database)
	{
		die ("Tidak dapat tersambung ke Host atau database tidak ditemukan!");
	}

	$soal = $_POST["soalPost"];
	$jwb = $_POST["jwbPost"];

	$jaw = "select * from hidden where soal = '$soal' AND jwb = '$jwb'";

	$result = mysqli_query($connect, $jaw);

	if ($result)
	{
		echo "Dapet nilai";
		$nilai = "update user set hiddenscore = hiddenscore + 10 where id = 1";
		$score = mysqli_query($connect, $nilai);
	}
	else
	{
		echo "INVALID";
	}
	?>
	s
