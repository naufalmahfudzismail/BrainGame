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

    $sql = "select tipe_katadasar from tb_katadasar";
    $result = mysqli_query($conn, $sql);

    $storeArray =  Array();

    if( mysqli_num_rows($result) > 0){

      while ($row = mysqli_fetch_assoc($result))
      {
          $storeArray[] = $row['tipe_katadasar'];
      }

      $countArr = count($storeArray);

      for( $x = 0; $x < $countArr; $x ++)
      {
          echo $storeArray[$x];
          echo "|";
      }

    }
 ?>
