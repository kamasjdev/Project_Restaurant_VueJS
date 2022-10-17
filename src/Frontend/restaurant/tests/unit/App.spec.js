import {  mount  } from '@vue/test-utils'
import App from '@/App.vue'

const store = {
    getters: {
        user: null
    }
}

describe('App', () => {
  it('should render App', () => {
    const msg = 'Kamil Wojtasiński';
    const wrapper = mount(App, { global: { mocks: { $store: store } } });
    expect(wrapper.text()).toMatch(msg);
  })
})