<?php
require_once 'db_connect.php';
require_once 'matching.php';
require_once 'session_manager.php';
$roomId = $_POST['room_id'];

$matching = new Matching($db);
$sessionManager = new SessionManager($db, $roomId);

$isConnected = $matching->is_connect_players($roomId);
if($isConnected){
    $sessionManager->room->update_room_status(RoomStatus::PLAYING);
    $sessionManager->initialize_users();
}

echo $sessionManager->room->get_room_status();
?>