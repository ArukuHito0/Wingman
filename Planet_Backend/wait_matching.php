<?php
require_once 'db_connect.php';
require_once 'matching.php';
$matching = new Matching($db);

$roomId = $_POST['room_id'];
$isConnected = $matching->is_connect_players($roomId);
if($isConnected){
    $matching->start_game($roomId);
}

echo $isConnected ? RoomStatus::PLAYING : RoomStatus::WAITING;
?>