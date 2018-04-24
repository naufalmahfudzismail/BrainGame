<?php

  $host = "localhost";
  $dbName = "katadasar";
  $user = "root";
  $password = "";

  $conn = new mysqli($host, $user, $password, $dbName);


  $katdas = $_POST["katadasarPost"];
  $katdas = mysqli_real_escape_string($conn, $katdas);
  $kalimat = $_POST["kalimatPost"];
  $kalimat = mysqli_real_escape_string($conn, $kalimat);


  if(!$conn)
  {
      die("Connection Failed. ".mysqli_connect_error());
  }

  $sql = "INSERT INTO tb_kalimat (katadasar, kalimat, Date) values ('$katdas', '$kalimat', now())";

  $result  = mysqli_query($conn, $sql);

  if(!$result)
  {
     echo "Terdapat Error pada server";
  }

  else echo "It's Okay"

?>
