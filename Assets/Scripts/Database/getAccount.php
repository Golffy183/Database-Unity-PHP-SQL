<?php

$db = "winner_admin_01";//Your database name
$dbu = "winner_admin_01";//Your database username
$dbp = "1234";//Your database users' password
$host = "localhost";//MySQL server - usually localhost

// Create connection
$conn = new mysqli($host, $dbu, $dbp, $db);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}


// Retrieve data from database

// $sql = "SELECT ID, PLAYER_NAME, SCORE, ZOMBIE FROM information ORDER BY SCORE DESC LIMIT 10";
$sql = "SELECT Account_ID, FName, LName, Coin FROM Account";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    echo "" . $row["Account_ID"]. " " . $row["FName"]. " " . $row["LName"]. " " . $row["Coin"]. "<br>";
  }
} else {
  echo "";
}

// close MySQL connection 
$conn->close();
?>