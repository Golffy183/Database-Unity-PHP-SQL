<?php

$db = "winner_****_**";//Your database name
$dbu = "winner_****_**";//Your database username
$dbp = "****";//Your database users' password
$host = "localhost";//MySQL server - usually localhost

// Create connection
$conn = new mysqli($host, $dbu, $dbp, $db);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$Account_ID = $_POST['Account_ID'];
$Item_ID = $_POST['Item_ID'];
$Item_Price = $_POST['Item_Price'];

// get piece of item
$sql = "SELECT Piece FROM Inventory WHERE Item_ID = '$Item_ID' AND Account_ID = '$Account_ID'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  $row = $result->fetch_assoc();
  $Piece = $row["Piece"];
} else {
  echo "";
}

// update piece when buy item
$Piece = $Piece + 1;
$sql = "UPDATE Inventory SET Piece = '$Piece' WHERE Item_ID = '$Item_ID' AND Account_ID = '$Account_ID'";
$result = $conn->query($sql);

// update coin when buy item
$sql = "SELECT Coin FROM Account WHERE Account_ID = '$Account_ID'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  $row = $result->fetch_assoc();
  $Coin = $row["Coin"];
} else {
  echo "";
}

// minus coin when buy item
$Coin = $Coin - $Item_Price;
$sql = "UPDATE Account SET Coin = '$Coin' WHERE Account_ID = '$Account_ID'";
$result = $conn->query($sql);

echo "" . $Coin;

$conn->close();
?>