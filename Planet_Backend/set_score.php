<?php
require_once 'db_connect.php';
require_once 'user.php';

$playerId = $_POST['player_id'];
$score = $_POST['score'];

$user = new User($db, $playerId);
$user->update_best_score($score);
?>