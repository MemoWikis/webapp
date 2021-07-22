
const {
  createRouter,
  createMemoryHistory,
  createWebHistory,
} = require('vue-router');

const isServer = typeof window === 'undefined';

let history = isServer ? createMemoryHistory() : createWebHistory();

const routes = [
  {
     path: '/',
     redirect: '/Dashboard',
  },
];

export function _createRouter() {
  return createRouter({ routes, history });
}
