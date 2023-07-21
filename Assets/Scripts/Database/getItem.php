<?php

$db = "winner_admin_01"; //Your database name
$dbu = "winner_admin_01"; //Your database username
$dbp = "1234"; //Your database users' password
$host = "localhost"; //MySQL server - usually localhost

// Create connection
$conn = new mysqli($host, $dbu, $dbp, $db);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}


// $Market_ID = $_POST['Market_ID'];
$Market_ID = $_POST['Market_ID'];

// $sql = "SELECT Market_ID, Name FROM Market";
$sql = "SELECT Item_ID, Name, Description, Price FROM Item WHERE Market_ID = '$Market_ID'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while ($row = $result->fetch_assoc()) {
    echo "" . $row["Item_ID"] . "-" . $row["Name"] . "-" . $row["Description"] . "-" . $row["Price"] . ",";
  }
} else {
  echo "";
}

// close MySQL connection 
$conn->close();
?>