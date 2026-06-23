<?php
require_once 'db_connect.php';
require_once 'user_resolver.php';
require_once 'user.php';

$userResolver = new UserResolver($db);

for($i = 0; $i < 100; $i++){
    $userId = $userResolver->create_user_id();
    $userIdKey = $userResolver->resolve_id($userId);

    $user = new User($db, $userIdKey);
    $user->update_best_score(random_int(1000, 99999));  // ランダムなスコアを設定
}
?>