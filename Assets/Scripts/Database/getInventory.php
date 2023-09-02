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

$sql = "SELECT Item_ID, Piece FROM Inventory WHERE Account_ID = '$Account_ID'";
$result = $conn->query($sql);

// get item name, description, price
if ($result->num_rows > 0) {
  // output data of each row
  $i = 0;
  while ($row = $result->fetch_assoc()) {
    $Item_ID[$i] = $row["Item_ID"];
    $Piece[$i] = $row["Piece"];
    $i++;
  }
} else {
  echo "";
}

// get item name, description, price
for ($j = 0; $j < $i; $j++) {
  $sql = "SELECT Name, Description, Price FROM Item WHERE Item_ID = '$Item_ID[$j]'";
  $result = $conn->query($sql);

  if ($result->num_rows > 0) {
    // output data of each row
    $row = $result->fetch_assoc();
    $Name[$j] = $row["Name"];
    $Description[$j] = $row["Description"];
    $Price[$j] = $row["Price"];
    // if have item piece
    if ($Piece[$j] > 0) {
      echo "" . $Item_ID[$j] . "-" . $Name[$j] . "-" . $Description[$j] . "-" . $Piece[$j] . "-" . $Price[$j] . ",";
    }
  } else {
    echo "";
  }
}

// close MySQL connection 
$conn->close();
?>