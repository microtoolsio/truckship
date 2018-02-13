//alue
//  :
//  access_token
//    :
//    "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vZGV2YXBpLnByb21hbmFnZXBsYW4uY29tL3YxL2F1dGgvbG9naW4iLCJpYXQiOjE1MTc4MjQ2MzUsImV4cCI6MTUxNzgyODIzNSwibmJmIjoxNTE3ODI0NjM1LCJqdGkiOiJjamJsMzVuZ2RuaVRDVlVQIiwic3ViIjoxLCJwcnYiOiI4N2UwYWYxZWY5ZmQxNTgxMmZkZWM5NzE1M2ExNGUwYjA0NzU0NmFhIiwidmlkIjoiMTIzNDU2NzgifQ.u2ZARh4SpZhTIxkYh4vZCbs12Pb8acon_pMd_VBe0xM"
//expires_in
//  :
//  3600
//token_type
//  :
//  "bearer"
export class Jwt {
  constructor(public access_token: string, public expires_in: number, public  token_type: string) { }
}
