<?php

  $host = "localhost";
  $dbName = "katadasar";
  $user = "root";
  $password = "";

    $conn = new mysqli($host, $user, $password, $dbName);

    if(!$conn)
    {
        die("Connection Failed. ".mysqli_connect_error());
    }


  $user = $_POST["username"];
  $user = mysqli_real_escape_string($conn, $user);
  $pass = $_POST["password"];
  $pass = mysqli_real_escape_string($conn, $pass);


    $sql = "SELECT * FROM data_user where ( username = '$user') and (password = '$pass')";

  $result = mysqli_query($conn, $sql);

  if( !$result)
  {
     echo "Masalah Query";
  }

  $rowcount = mysqli_num_rows($result);

    if ($rowcount == 1)
    {
      echo "benar";
    }
    else
    {
      echo "salah";
    }


 ?>
