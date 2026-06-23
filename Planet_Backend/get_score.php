<?php
require_once 'db_connect.php';
require_once 'user.php';

$playerId = $_POST['player_id'];

$user = new User($db, $playerId);
echo $user->get_best_score();
?>