<?php
require_once 'db_connect.php';

$planet_history = $_POST['planet_history'];
$playerId = $_POST['player_id'];

$stmt = $db->prepare("UPDATE users SET planet_history = :planet_history WHERE id = :playerId");
$stmt->bindValue(':planet_history', $planet_history, PDO::PARAM_STR);
$stmt->bindValue(':playerId', $playerId, PDO::PARAM_INT);
$stmt->execute();
?>