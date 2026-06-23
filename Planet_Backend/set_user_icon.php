<?php
require_once 'db_connect.php';
require_once 'user.php';

$playerId = $_POST['player_id'];
$userIcon = $_POST['user_icon'];

$user = new User($db, $playerId);
$user->update_user_icon($userIcon);
?>