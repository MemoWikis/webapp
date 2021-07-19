
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
     redirect: '/news',
  },
];

export function _createRouter() {
  return createRouter({ routes, history });
}
