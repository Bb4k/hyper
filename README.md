# hyper

## APIs

### GET
- [x] GET: users
- [x] GET: user by id
- [x] GET: feed (postare + nr comentarii + nr hypes)
- [x] GET: profile + postari + UserPRs + count_friends
- [x] GET: pr by id
- [x] GET: comentariile unei postari + profile_pic/username
- [x] GET: friend requests (poza, username)
- [x] GET: search friend by username (return name, picture, are_friends)

### POST
- [x] POST: register (picture, email, username, password, height, weight)
- [x] POST: login (username, password)
- [x] POST: send friend request (status: 0:pending; 1:friends)
- [x] POST: postare 
- [x] POST: comentariu
- [x] POST: PR (hardcoded in db, useless for frontend)
- [x] POST: hype
- [x] POST: unhype

### PUT
- [x] PUT: accept friend
- [x] PUT: update user (picture, email, weight, height)

### DELETE
- [x] DELETE: user delete by id
- [x] DELETE: comentariu
- [x] DELETE: reject friend
- [x] DELETE: posts

- [x] adaugat roluri la users (admin, registered, guest)
- [x] adaugat public/private profile users (private: 0/1)
- [x] edit api edit profile public/private
- [x] modificat api user profile sa zica daca sunt prieteni deja sau nu (are_friends: 0 - no, 1 - yes, 2 - pending)
- [x] crearea conturi admin + guest
- [ ] status de accepted la comentarii
- [ ] modificat API de get comments sa arate doar alea accepted
- [ ] de adaugat si comentariile in pending la API ul de cerere de prietenie
- [ ] de adaugat si warnings la API ul de cerere de prietenie
- [ ] de vazut cu mai multe poze per postare
- [ ] inbox pentru mesaje

- [x] Populate database script








