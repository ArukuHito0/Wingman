<?php
require_once 'db_connect.php';
require_once 'user.php';

$playerId = $_POST['player_id'];

$user = new User($db, $playerId);
$iconNumber = $user->get_user_icon();

echo $iconNumber;
?>