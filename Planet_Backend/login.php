<?php
// ユーザーの情報を取得するAPI
require_once 'db_connect.php';
require_once 'user_resolver.php';
require_once 'user.php';

$userId = $_POST['user_id'];
$userResolver = new UserResolver($db);

$id = $userResolver->resolve_id($userId);    // 渡されたユーザーIDのDBのIDを受け取る

$user = new User($db, $id);
$data = $user->get_user_data();

echo json_encode([
    'player_id' => $data['id'],
    'user_id' => $data['user_id']
]);
?>