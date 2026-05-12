<?php
require_once 'db_connect.php';
require_once 'matching.php';
require_once 'user.php';
$matching = new Matching($db);
$user = new User($db);
$room = null;

$password = $_POST['password'];     // Unityで設定した合言葉
$userId = $_POST['user_id'];

$id = $user->login($userId);    // 渡されたユーザーIDのDBのIDを受け取る

// 合言葉が空文字だったら無し同士でランダムマッチ
if(empty($password)){
    $roomId = $matching->find_free_room();
}else{
    $roomId = $matching->find_password_room($password);
}

// 一致するルームが無い場合、自分でルームを作成
if($roomId == 0){
    $roomId = $matching->create_room($password);
}

if($roomId != 0){
    $matching->join_room($roomId, $id);
}

// 最新のユーザーデータを取得
$row = $user->get_user_data($id);

echo json_encode([
    'user_id' => $row['user_id'],
    'player_id' => $row['id'],
    'room_id' => $row['room_id']
    ]);
?>