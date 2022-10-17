import {  mount, RouterLinkStub } from '@vue/test-utils'
import Home from '@/pages/Home.vue'

const store = {
    getters: {
        user: null
    }
}

describe('Home page', () => {
    it('should render Home page', () => {
      const msg = 'Witaj w Restaurant App';
      const wrapper = mount(Home, { global: { mocks: { $store: store }, stubs: { RouterLink: RouterLinkStub } } });
      expect(wrapper.text()).toMatch(msg);
    })
})