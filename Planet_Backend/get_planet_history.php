<?php
require_once 'db_connect.php';

$planet_history = $_POST['planet_history'];
$playerId = $_POST['player_id'];

$stmt = $db->prepare("SELECT planet_history FROM users WHERE id = :playerId");
$stmt->bindValue(':playerId', $playerId, PDO::PARAM_INT);
$stmt->execute();
$result = $stmt->fetch(PDO::FETCH_ASSOC);

echo json_encode($result);
?>