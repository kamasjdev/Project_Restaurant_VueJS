import { shallowMount, mount, flushPromises, createLocalVue } from '@vue/test-utils'
import Login from '@/pages/Login.vue'
import axios from '@/axios-setup.js'
import store from '@/vuex.js'

describe('Login page', () => {
  it('should render Login page', () => {
    const msg = 'Logowanie';
    const wrapper = shallowMount(Login);
    expect(wrapper.text()).toMatch(msg);
  })

  it('given invalid password and email should show error on inputs', async () => {
    const wrapper = mount(Login);
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    await wrapper.vm.$nextTick(); // invoke again component
    
    expect(loginButton).not.toBeNull();
    const validationErrors = wrapper.findAll('div.invalid-feedback');
    expect(validationErrors).not.toBeNull();
    expect.any(validationErrors);
    expect(validationErrors).toHaveLength(2);
    expect(validationErrors.find(v => v.html().includes('Hasło')).text()).toMatch('Hasło nie może być puste');
    expect(validationErrors.find(v => v.html().includes('Email')).text()).toMatch('Email nie może być pusty');
  })

  it('given invalid email should show error', async () => {
    const wrapper = mount(Login);
    await setEmailAndPasswordInInputs(wrapper, 'abc', '123');
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    await wrapper.vm.$nextTick(); // invoke again component
    
    expect(loginButton).not.toBeNull();
    const validationErrors = wrapper.findAll('div.invalid-feedback');
    expect(validationErrors).not.toBeNull();
    expect.any(validationErrors);
    expect(validationErrors).toHaveLength(1);
    expect(validationErrors.find(v => v.html().includes('email')).text()).toMatch('Niepoprawny email');
  })

  it('given bad request when send to backend should show error', async () => {
    jest.spyOn(axios, 'post').mockImplementation(() => Promise.reject({
      response: {
        status: 400,
        data: {
          code: 'invalid_credentials'
        }
      }
    }));
    const wrapper = mount(Login);
    await setEmailAndPasswordInInputs(wrapper, 'email@email.com', '123');
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    
    // Wait until the DOM updates.
    await flushPromises();

    expect(wrapper.find('div.alert.alert-danger')).not.toBeNull();
    expect(wrapper.text()).toMatch('Niepoprawne dane logowania');    
  })

  it('given internal error server should show error', async () => {
    jest.spyOn(axios, 'post').mockImplementation(() => Promise.reject({
      response: {
        status: 500
      }
    }));
    const wrapper = mount(Login);
    await setEmailAndPasswordInInputs(wrapper, 'email@email.com', '123');
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    
    // Wait until the DOM updates.
    await flushPromises();

    expect(wrapper.find('div.alert.alert-danger')).not.toBeNull();
    expect(wrapper.text()).toMatch('Wystąpił błąd');    
  })

  it('given invalid response should show error', async () => {
    jest.spyOn(axios, 'post').mockImplementation(() => Promise.reject({}));
    const wrapper = mount(Login);
    await setEmailAndPasswordInInputs(wrapper, 'email@email.com', '123');
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    
    // Wait until the DOM updates.
    await flushPromises();

    expect(wrapper.find('div.alert.alert-danger')).not.toBeNull();
    expect(wrapper.text()).toMatch('Coś poszło nie tak');    
  })

  it('given valid data shouldnt show error', async () => {
    jest.spyOn(axios, 'post').mockImplementation(() => Promise.resolve({
        status: 200,
        data: {
          expiry: Number(100000)
        }
    }));
    const mockRouter = {
      push: jest.fn()
    }
    const wrapper = mount(Login, { global: { mocks: { $store: store, $router: mockRouter } }});
    await setEmailAndPasswordInInputs(wrapper, 'email@email.com', '123');
    const loginButton = wrapper.find('button.btn.btn-success');

    loginButton.trigger('click');
    
    // Wait until the DOM updates.
    await flushPromises();

    expect(wrapper.find('div.alert.alert-danger').exists()).toBe(false);
    expect(mockRouter.push).toHaveBeenCalledWith('/')
  })
})

async function setEmailAndPasswordInInputs(wrapper, email, password) {
  await wrapper.get("#Email-input").setValue(email);
  await wrapper.get("#Haslo-input").setValue(password);
}
