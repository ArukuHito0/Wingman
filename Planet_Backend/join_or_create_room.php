<?php
require_once 'db_connect.php';
require_once 'matching.php';
require_once 'user.php';
require_once 'user_resolver.php';

$password = $_POST['password'];     // Unityで設定した合言葉
$userName = $_POST['user_name'];   // Unityで設定したユーザー名
$userId = $_POST['user_id'];

$matching = new Matching($db);
$userResolver = new UserResolver($db);
$room = null;

$id = $userResolver->resolve_id($userId);    // 渡されたユーザーIDのDBのIDを受け取る

$user = new User($db, $id);
$user->update_user_name($userName);  // ユーザー名を更新

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
$row = $user->get_user_data();

echo json_encode([
    'user_id' => $row['user_id'],
    'player_id' => $row['id'],
    'room_id' => $row['room_id']
    ]);
?>