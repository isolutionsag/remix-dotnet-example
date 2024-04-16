interface Token {
  token: string;
  expiresAt: string;
}

export interface User {
  displayname: string;
  username: string;
  firstname: string;
  lastname: string;
  roles: string[];
}

export interface UserWithTokens extends User {
  tokens: {
    access: Token;
    refresh: Token;
  };
}
