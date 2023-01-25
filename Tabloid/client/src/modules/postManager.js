import { getToken } from "./authManager";

const apiUrl = "/api/post";

export const getAllPosts = () => {
  return getToken().then((token) => {
    return fetch(apiUrl, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }).then((resp) => {
      if (resp.ok) {
        return resp.json();
      } else {
        throw new Error(
          "An unknown error occurred while trying to get posts.",
        );
      }
    });
  });
};

export const getUserPosts = () => {
  return getToken().then((token) => {
    return fetch(`${apiUrl}/userposts`, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }).then((resp) => {
      if (resp.ok) {
        return resp.json();
      } else {
        throw new Error(
          "An unknown error occurred while trying to get posts.",
        );
      }
    });
  });
};